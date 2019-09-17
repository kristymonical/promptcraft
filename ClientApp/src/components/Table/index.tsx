import React from 'react';
import { Table as RbsTable } from 'react-bootstrap';
import { makeStyles } from '@material-ui/core';
import { SVT_THEME } from 'components';

interface TableProps {
  data: any[] | any;
  maxWidth?: string;
  shape: { label: string; key: string }[];
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
  tableItem: {
    background: 'white',
    border: '1px solid #D5D5D5 !important', // @styles figure out how to remove !important
    borderRadius: 5,
    color: 'black'
  }
}));

export default function Table({ data, shape, maxWidth }: TableProps) {
  const classes = useStyles({ maxWidth });

  return (
    <RbsTable className={classes.tableRoot}>
      <thead>
        <tr>
          {shape.map(({ label, key }, idx) => (
            <th className={classes.tableHeaderItem} key={`${key}-${idx}`}>
              {label}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {Array.isArray(data) ? (
          data.map((datum, rowIdx) => (
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
          ))
        ) : (
          <tr>
            {shape.map(({ key }, idx) => (
              <td className={classes.tableItem} key={`${key}-C${idx}`}>
                {data[key]}
              </td>
            ))}
          </tr>
        )}
      </tbody>
    </RbsTable>
  );
}
