import React  from 'react';
import { Route, Routes } from 'react-router-dom';

import { createTheme, ThemeProvider, styled } from '@mui/material/styles';

import Login from './components/Login';
import Layout from './components/Layout';

const App = () => {
    const [logged, setLogged] = React.useState(false);
    const [userId, setUserId] = React.useState(0);

    const theme = createTheme({
        palette: {
            primary: {
                main: '#00DFFC',
            },
            secondary: {
                main: '#00B4CC',
            },
            middle: {
                main: '#008C9E',
            },
            force: {
                main: '#005F6B',
            },
            black: {
                main: '#343838',
            }
        }
    });

    const login = (data) => {
        setLogged(true);
        setUserId(data["Id"]);
    }

    return (
       logged ? <Layout userid={userId} /> : <Login logged={login} />
    );
}

export default App;