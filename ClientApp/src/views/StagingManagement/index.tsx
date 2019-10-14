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
import { batchCreateDeliveryRequests } from 'services/Delivery';

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
      const promises: Promise<GetAreasResult[]>[] = [];
      rows.forEach(row => {
        promises.push(getDestinationAreas(row.stagingLocationId));
      });

      try {
        const results = _.flatten(await Promise.all(promises));
        const destinationAreas = _.unionBy(_.flatten(results), 'areaId').map(
          area => area.areaName
        );

        if (destinationAreas.length === 0) {
          toast.error('No common destination area found for selection.');
        }

        setAvailableFinalDestinations(destinationAreas);
      } catch {
        setAvailableFinalDestinations([]);
      }
    },
    []
  );

  // get data on mount
  useEffect(() => {
    refreshStagedCarts();
  }, [refreshStagedCarts]);

  // submit batch delivery queue request and reset UI values
  const onSubmit = useCallback(() => {
    batchCreateDeliveryRequests(
      selectedRows.map(row => ({
        cartId: row.cartId,
        cartLocation: row.stagingLocationId,
        deliveryType: 'stage',
        destinationArea: finalDestination,
        orderNumber: row.orderId
      }))
    );
    setFinalDestination('');
    setSelectedRows([]);
  }, [finalDestination, setFinalDestination, setSelectedRows, selectedRows]);

  return (
    <>
      <Row>
        <TitleCol title='Staging Management'>
          <Typography>
            Use this screen to add or modify the Destination Area for staged
            carts and create new Delivery Requests for loaded and unloaded
            carts.
          </Typography>
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
        <AutoRefresh callback={refreshStagedCarts} />
      </Row>
      {stagingTableData.length > 0 ? (
        <>
          <Row className={classes.overlayContainer}>
            {locked && <Overlay />}
            <Table
              data={stagingTableData}
              noMargins
              onSelectRow={selected => setSelectedRows(selected)}
              selectable
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
