import React from 'react';
import { ThemeProvider as MuiThemeProvider } from '@material-ui/styles';
import { createMuiTheme } from '@material-ui/core/styles';

const theme = createMuiTheme({
  typography: {
    fontFamily: ['"Helvetica Neue"'].join(',')
  }
});

export default function ThemeProvider({ children }: { children: any }) {
  return <MuiThemeProvider theme={theme}>{children}</MuiThemeProvider>;
}
