import React from 'react';
import { Link, Route, Routes } from 'react-router-dom';

import { Sidebar, Menu, MenuItem } from 'react-pro-sidebar';

import { Box } from '@mui/material';

import AccountTreeIcon from '@mui/icons-material/AccountTree';

import AppRoutes from '../AppRoutes';

const Layout = (props) => {
    const { userid } = props;

    return (
        <div id="app" className="wh100">
            <Sidebar backgroundColor="#fcffff">
                <Box sx={{ width: '100%', heigth: '100vh' }}>
                    <Box className="menuTitle">Geral</Box>
                    <Menu menuItemStyles={{
                        button: {
                            color: '#008C9E',
                            "&:hover": {
                                backgroundColor: 'rgba(0, 223, 252, 0.1) !important',
                                fontSize: '18px',
                            },
                        },
                    }}>
                        <MenuItem className="sideMenuItem" icon={<AccountTreeIcon/>} component={<Link to="/" />}>Projeto</MenuItem>
                    </Menu>
                </Box>
            </Sidebar>
            <div id="content">
                <Routes>
                    {
                        AppRoutes(userid).map((route, index) => {
                            const { element, ...rest } = route;
                            return (<Route key={index} {...rest} element={element} />);
                        })
                    }
                </Routes>
            </div>
        </div>
    );
}

export default Layout;