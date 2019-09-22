import React, { useEffect, useState } from 'react';
import { Table as RbsTable } from 'react-bootstrap';
import { makeStyles, Typography } from '@material-ui/core';
import { SVT_THEME } from 'components';
import _ from 'lodash/fp';

import FilterPopover from './FilterPopover';

interface ColumnShape {
  filter?: boolean;
  key: string;
  label: string;
}

interface Filters {
  [key: string]: {
    [value: string]: boolean;
  };
}

export interface TableProps<TData = any> {
  data: TData[];
  maxWidth?: string;
  onSelectRow?: (selectedRows: TData[]) => void;
  shape: ColumnShape[];
}

function getActiveFilters(filters: Filters) {
  const activeFilters: Filters = {};

  Object.keys(filters).forEach(column =>
    Object.keys(filters[column]).forEach(value => {
      // filter not toggled, skip it
      if (!filters[column][value]) return;

      // filter is toggled, add it to active
      if (!activeFilters[column]) {
        activeFilters[column] = { [value]: true };
      } else {
        activeFilters[column][value] = true;
      }
    })
  );

  return activeFilters;
}

const useStyles = makeStyles(({ primary }: typeof SVT_THEME) => ({
  tableRoot: {
    // CSS hack to do rounded borders that look collapsed
    borderCollapse: 'initial',
    borderSpacing: 0,
    marginTop: '1rem', // @styles add props to easily define margins
    maxWidth: ({ maxWidth }: Partial<TableProps>) => maxWidth || 'initial',
    '& tbody tr': {
      cursor: ({ onSelectRow }: Partial<TableProps>) =>
        typeof onSelectRow === 'function' ? 'pointer' : 'inherit'
    }
  },
  tableHeaderItem: {
    background: primary.background,
    border: '1px solid #D5D5D5 !important', // @styles figure out how to remove !important
    borderRadius: 5,
    color: 'white'
  },
  tableHeaderItemFlex: {
    display: 'flex',
    justifyContent: 'space-between'
  },
  tableItem: {
    background: 'white',
    border: '1px solid #D5D5D5 !important', // @styles figure out how to remove !important
    borderRadius: 5,
    color: 'black'
  },
  selectedRow: {
    '& td': {
      background: `${primary.background}50`
    }
  }
}));

export default function Table({
  data,
  maxWidth,
  onSelectRow,
  shape
}: TableProps) {
  const classes = useStyles({ maxWidth, onSelectRow });

  const [filters, setFilters] = useState<Filters>({}); // filters for display purposes
  const [filteredData, setFilteredData] = useState<typeof data>(data); // filtered data
  const [selectedRows, setSelectedRows] = useState<number[]>([]); // selected rows

  // update/reset filters when data or shape changes
  useEffect(() => {
    // no data means no filters
    if (data.length === 0) {
      setFilters({});
    } else {
      const newFilters: Filters = {};
      shape
        .filter(col => col.filter) // filter out non-filtered columns
        .map(col => col.key) // map filtered columns to array of strings of column keys
        .forEach(filterableColumnKey => {
          _.uniq(data.map(datum => `${datum[filterableColumnKey]}`)).forEach(
            uniqueDataValue => {
              if (!newFilters[filterableColumnKey]) {
                newFilters[filterableColumnKey] = {};
              }
              newFilters[filterableColumnKey][uniqueDataValue] = false;
            }
          );
        });

      setFilters(newFilters);
    }
  }, [data, shape]);

  // filter the data when it or the selected filters change
  useEffect(() => {
    // if any filter is toggled to true, we have active filters
    const hasFilters = Object.keys(filters).some(column =>
      Object.keys(filters[column]).some(
        filterValue => filters[column][filterValue]
      )
    );

    // if no filters are active, our filtered data is just our original data set
    if (!hasFilters) {
      setFilteredData(data);
    } else {
      // otherwise grab the active filters
      const activeFilters = getActiveFilters(filters);

      // filter data based on active filters
      const filteredData = data.filter(datum =>
        Object.keys(activeFilters).every(column =>
          Object.keys(activeFilters[column]).includes(datum[column])
        )
      );

      setFilteredData(filteredData);
    }
  }, [data, filters]);

  const onToggleFilter = (key: string) => (filterValue: string) =>
    setFilters(curFilters => {
      const newFilters = { ...curFilters };
      newFilters[key][filterValue] = !newFilters[key][filterValue];
      return newFilters;
    });

  const onTableRowClick = (rowIdx: number) => {
    return () => {
      setSelectedRows(curSelectedRows => {
        const selectedArrIdx = curSelectedRows.indexOf(rowIdx);
        let newArr: number[] = [];

        if (selectedArrIdx === -1) {
          // if row index is not in array, add it
          newArr = [...curSelectedRows, rowIdx];
        } else {
          // otherwise, remove it
          newArr = curSelectedRows.slice();
          newArr.splice(selectedArrIdx, 1);
        }
        typeof onSelectRow === 'function' &&
          onSelectRow(_.pick<number[]>(newArr, filteredData));
        return newArr;
      });
    };
  };

  return (
    <RbsTable className={classes.tableRoot}>
      <thead>
        <tr>
          {shape.map(({ label, key, filter }, idx) => (
            <th className={classes.tableHeaderItem} key={`${key}-${idx}`}>
              {(() => {
                const LabelContent = () => <Typography>{label}</Typography>;

                // if optional filter property is falsey only show the label
                if (!filter) return <LabelContent />;

                // filters exist for this column, show them
                return (
                  <div className={classes.tableHeaderItemFlex}>
                    <LabelContent />
                    <FilterPopover
                      filters={filters[key] || {}}
                      onToggleFilter={onToggleFilter(key)}
                    />
                  </div>
                );
              })()}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {filteredData.map((datum, rowIdx) => (
          <tr
            key={`${rowIdx}`}
            onClick={onTableRowClick(rowIdx)}
            className={selectedRows.includes(rowIdx) ? classes.selectedRow : ''}
          >
            {shape.map(({ key }, idx) => (
              <td
                className={classes.tableItem}
                key={`${key}-R${rowIdx}-C${idx}`}
              >
                {datum[key]}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </RbsTable>
  );
}
