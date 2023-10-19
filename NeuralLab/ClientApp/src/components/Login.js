import React from 'react';
import axios from 'axios';

import { Dialog, DialogTitle, InputAdornment, IconButton, OutlinedInput, InputLabel, FormControl, Button, CircularProgress, Alert, Box, Collapse } from '@mui/material';

import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import CloseIcon from '@mui/icons-material/Close';
import LoginIcon from '@mui/icons-material/Login';

import '../css/login.css';

function LoginDialog(props) {
    const { logged, theme } = props;
    let open = true;

    const [showPass, setShowPass] = React.useState(true);
    const [loading, setLoading] = React.useState(false);
    const [alert, setAlert] = React.useState('');

    const [account, setAccount] = React.useState('');
    const [password, setPassword] = React.useState('');

    const handleClickShowPassword = () => setShowPass(!showPass);

    const post = () => {
        setLoading(true);
        if (account == '') { setAlert('Nenhum acesso foi informado.'); setLoading(false); }
        else if (password == '') { setAlert('Nenhuma senha foi informada.'); setLoading(false); }

        const data = { Account: account, Password: password }
        axios.post('login', data)
            .then((response) => {
                const info = response["data"];
                console.log(response);
                if (info["Id"] < 0) {
                    setAlert(info["Message"]);
                    setLoading(false);
                }
                else {
                    logged(info);
                    setLoading(false);
                }
            });
    }

    return ( loading ? <CircularProgress sx={{marginLeft: '50vw', marginTop: '50vh', }}/> :
        <Dialog open={open}>
            <Collapse in={alert != ''} sx={{heigth: "48px"}}>
                <Alert
                    size="small"
                    sx={{width: "92%"}}
                    severity="error"
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => {
                                setAlert('');
                            }} >
                            <CloseIcon/>
                        </IconButton>
                    }
                >
                    { alert }
                </Alert>
            </Collapse>
            <DialogTitle sx={{ padding: '12px 16px', marginLeft: '16px' }}><span className="title">Login</span></DialogTitle>
            <FormControl sx={{ marginLeft: '4%', marginBottom: '16px', width: '92%' }} variant="outlined">
                <InputLabel htmlFor="account">Acesso</InputLabel>
                <OutlinedInput
                    id="account"
                    type={showPass ? 'text' : 'password'}
                    endAdornment={
                        <InputAdornment position="end">
                            <IconButton
                                aria-label="toggle password visibility"
                                onClick={handleClickShowPassword}
                                edge="end"
                            >
                                {showPass ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                        </InputAdornment>
                    }
                    label="Account"
                    onChange={e => setAccount(e.target.value)}
                />
            </FormControl>
            <FormControl sx={{ marginLeft: '4%', marginBottom: '16px', width: '92%' }} variant="outlined">
                <InputLabel htmlFor="password">Senha</InputLabel>
                <OutlinedInput
                    id="password"
                    type={showPass ? 'text' : 'password'}
                    endAdornment={
                        <InputAdornment position="end">
                            <IconButton
                                aria-label="toggle password visibility"
                                onClick={handleClickShowPassword}
                                edge="end"
                            >
                                {showPass ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                        </InputAdornment>
                    }
                    label="Password"
                    onChange={e => setPassword(e.target.value)}
                />
            </FormControl>

            <Button startIcon={<LoginIcon />} onClick={post} sx={{ marginLeft: '35%', marginRight: '35%', marginBottom: '16px' }} variant="outlined">Entrar</Button>
        </Dialog>
    );
}

const Login = (props) => {
    const { logged } = props;

    return (
        <div id="login">
            <LoginDialog logged={logged}/>
        </div>
    );
}

export default Login;