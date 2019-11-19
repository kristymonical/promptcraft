import React, { useState, useEffect, useCallback } from 'react';
import { Typography, Card, makeStyles } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';
import _ from 'lodash';

import { GetStagedCartsResult, getStagedCarts } from 'services/Staging';
import {
  TitleCol,
  SubmitButton,
  Table,
  AutoRefresh,
  Overlay,
  Button,
  Select
} from 'components';
import { getDestinationAreas, GetAreasResult } from 'services/Area';
import { toast } from 'react-toastify';
import {
  batchCreateDeliveryRequests,
  updateDeliveryDestinationArea
} from 'services/Delivery';

const cartsTableShape = [
  { label: 'Current Location', key: 'stagingLocationId' },
  { label: 'Order ID', key: 'orderId', filter: true },
  { label: 'Cart ID', key: 'cartId' },
  { label: 'Delivery Type', key: 'deliveryRequestType', filter: true }
];

const useStyles = makeStyles({
  actionRow: {
    alignItems: 'center',
    marginTop: '1rem',
    '& .col > *': {
      margin: '0 auto'
    },
    '& .col button': {
      display: 'block'
    }
  },
  emptyTableCard: {
    padding: 10,
    width: '100%',
    textAlign: 'center'
  },
  overlayContainer: {
    position: 'relative'
  }
});

export default function StagingManagement() {
  const [finalDestination, setFinalDestination] = useState('');
  const [stagingTableData, setStagingTableData] = useState<
    GetStagedCartsResult[]
  >([]);
  const [locked, setLocked] = useState(false);
  const [selectedRows, setSelectedRows] = useState<GetStagedCartsResult[]>([]);
  const [availableFinalDestinations, setAvailableFinalDestinations] = useState<
    string[]
  >([]);

  const classes = useStyles({});

  // get staged carts to show in table
  const refreshStagedCarts = useCallback(async () => {
    try {
      setLocked(true);

      const cartsData = await getStagedCarts();
      setStagingTableData(cartsData);
    } catch (err) {
    } finally {
      setLocked(false);
    }
  }, []);

  const getValidDestinationAreas = useCallback(
    async (rows: GetStagedCartsResult[]) => {
      setLocked(true);

      try {
        const promises: Promise<GetAreasResult[]>[] = [];
        rows.forEach(row => {
          promises.push(
            getDestinationAreas(
              row.stagingLocationId,
              row.deliveryRequestType === 'stage'
                ? 'deliver'
                : row.deliveryRequestType
            )
          );
        });

        const results = _.flatten(await Promise.all(promises));
        const destinationAreas = _.sortBy(
          _.unionBy(_.flatten(results), 'areaId').map(area => area.areaName)
        );

        if (destinationAreas.length === 0) {
          toast.error('No common destination area found for selection.');
          setAvailableFinalDestinations([]);
        } else {
          setAvailableFinalDestinations(destinationAreas);
        }
      } catch (err) {
        setAvailableFinalDestinations([]);
      } finally {
        setLocked(false);
      }
    },
    []
  );

  // get data on mount and if callback updates
  useEffect(() => {
    refreshStagedCarts();
  }, [refreshStagedCarts]);

  // submit batch delivery queue request and reset UI values
  const onSubmit = useCallback(async () => {
    setLocked(true);
    try {
      const batchDeliveriesPromise = batchCreateDeliveryRequests(
        selectedRows
          .filter(row => row.deliveryRequestType === 'stage')
          .map(row => ({
            cartId: row.cartId,
            cartLocation: row.stagingLocationId,
            deliveryType: 'deliver',
            destinationArea: finalDestination,
            orderNumber: row.orderId
          }))
      );

      const updateDestinationPromises = selectedRows
        .filter(row => row.deliveryRequestType !== 'stage')
        .map(row =>
          updateDeliveryDestinationArea(row.deliveryId, finalDestination)
        );

      await Promise.all([...updateDestinationPromises, batchDeliveriesPromise]);

      refreshStagedCarts();
      setFinalDestination('');
      setSelectedRows([]);
      setAvailableFinalDestinations([]);
    } catch (err) {
    } finally {
      setLocked(false);
    }
  }, [finalDestination, selectedRows, refreshStagedCarts]);

  return (
    <>
      <Row>
        <TitleCol title='Staging Management'>
          <Typography>Use this screen to deliver staged carts.</Typography>
          <Typography>
            <b>Suite MAL delivery requests:</b> Filter list of staged carts by
            Delivery Request Type and/or Order ID and select new Suite MAL
            destination to create new Delivery Request.
          </Typography>
          <Typography>
            <b>Return Cart Delivery Requests:</b> Filter list of staged carts by
            Delivery Request Type and select staged Delivery Requests to modify
            and select FPA or CARWASH destination to create new Delivery Request
            for a cart return.
          </Typography>
        </TitleCol>
      </Row>
      <Row>
        <AutoRefresh
          callback={() => selectedRows.length === 0 && refreshStagedCarts()}
        />
      </Row>
      {stagingTableData.length > 0 ? (
        <>
          <Row className={classes.overlayContainer}>
            {locked && <Overlay />}
            <Table
              data={stagingTableData}
              dataIdField='cartId'
              noMargins
              onSelectRow={selected => {
                setSelectedRows(selected);
                setFinalDestination('');
                setAvailableFinalDestinations([]);
              }}
              selectable
              selectedRows={selectedRows}
              shape={cartsTableShape}
            />
          </Row>
          <Row className={classes.actionRow}>
            <Col>
              <Button
                disabled={selectedRows.length === 0 || locked}
                maxWidth={250}
                onClick={() => getValidDestinationAreas(selectedRows)}
                scale={1.25}
              >
                Confirm Selection
              </Button>
            </Col>
            <Col>
              {availableFinalDestinations.length > 0 && (
                <Select
                  handleChange={newValue => setFinalDestination(newValue)}
                  items={availableFinalDestinations}
                  label={<Typography variant='button'>Destination</Typography>}
                  value={finalDestination}
                />
              )}
            </Col>
            <Col>
              {finalDestination.length > 0 && (
                <SubmitButton
                  onClick={onSubmit}
                  text='Create Staging Request'
                  variant='secondary'
                />
              )}
            </Col>
          </Row>
        </>
      ) : (
        <Card className={classes.emptyTableCard}>
          <Typography>No staged carts found.</Typography>
        </Card>
      )}
    </>
  );
}
