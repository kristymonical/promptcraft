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

interface Filter {
  key: string;
  values: {
    value: string;
    checked: boolean;
  }[];
}

export interface TableProps {
  data: any[];
  maxWidth?: string;
  shape: ColumnShape[];
}

const useStyles = makeStyles(({ primary }: typeof SVT_THEME) => ({
  tableRoot: {
    // CSS hack to do rounded borders that look collapsed
    borderCollapse: 'initial',
    borderSpacing: 0,
    marginTop: '1rem', // @styles add props to easily define margins
    maxWidth: ({ maxWidth }: Partial<TableProps>) => maxWidth || 'initial'
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
  }
}));

export default function Table({ data, shape, maxWidth }: TableProps) {
  const classes = useStyles({ maxWidth });

  const [filters, setFilters] = useState<Filter[]>([]);

  // update/reset filters when data or shape changes
  useEffect(() => {
    if (data.length === 0) {
      setFilters([]);
    } else {
      const newFilters = shape
        .filter(col => col.filter)
        .map(col => col.key)
        .map(key => ({
          key,
          values: _.uniq(data.map(datum => `${datum[key]}`)) // get unique values coersed into strings to make types play nice
            .map(value => ({ value, checked: false })) // map into filter format
        }));

      setFilters(newFilters);
    }
  }, [data, shape]);

  return (
    <RbsTable className={classes.tableRoot}>
      <thead>
        <tr>
          {shape.map(({ label, key, filter }, idx) => (
            <th className={classes.tableHeaderItem} key={`${key}-${idx}`}>
              {(() => {
                const LabelContent = () => <Typography>{label}</Typography>;
                if (!filter) return <LabelContent />;

                const colFilter = filters.find(
                  (filter: Filter) => filter.key === key
                );
                if (!colFilter) return <LabelContent />;

                return (
                  <div className={classes.tableHeaderItemFlex}>
                    <LabelContent />
                    <FilterPopover
                      filters={colFilter.values}
                      onToggleFilter={() => {}}
                    />
                  </div>
                );
              })()}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {data.map((datum, rowIdx) => (
          <tr key={`${rowIdx}`}>
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
