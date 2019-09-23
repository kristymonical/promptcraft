import React from 'react';
import { ThemeProvider as MuiThemeProvider } from '@material-ui/styles';
import { createMuiTheme } from '@material-ui/core/styles';
import { CssBaseline } from '@material-ui/core';

export const SVT_THEME = {
  initial: {
    fontSize: 14
  },
  flex: {
    horizontalSpacing: 25,
    verticalSpacing: 10
  },
  primary: {
    background: '#405A74',
    color: 'white',
    dark: '#2C3E50'
  },
  secondary: {
    background: '#E67E22',
    color: 'white'
  }
};

const baseTheme = createMuiTheme({
  palette: {
    background: {
      default: '#EEEEEE'
    }
  },
  typography: {
    fontFamily: '"Helvetica Neue"',
    body1: {
      fontSize: 14
    },
    h5: {
      fontSize: '26px'
    },
    h3: {
      fontSize: '42px'
    }
  }
});

export default function ThemeProvider({ children }: { children: any }) {
  return (
    <MuiThemeProvider theme={baseTheme}>
      <MuiThemeProvider theme={SVT_THEME}>
        <CssBaseline />
        {children}
      </MuiThemeProvider>
    </MuiThemeProvider>
  );
}
