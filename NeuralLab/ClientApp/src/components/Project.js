import React, { useState } from 'react';
import axios from 'axios';

import { Box, Grid, Paper, Button, Dialog, DialogTitle, Alert, Collapse, CircularProgress, InputAdornment, IconButton, OutlinedInput, InputLabel, FormControl, MenuItem, Select } from '@mui/material';

import AccountTreeIcon from '@mui/icons-material/AccountTree';
import PersonIcon from '@mui/icons-material/Person';
import SpokeIcon from '@mui/icons-material/Spoke';
import AddIcon from '@mui/icons-material/Add';
import ShowChartIcon from '@mui/icons-material/ShowChart';
import CloseIcon from '@mui/icons-material/Close';
import FolderOpenIcon from '@mui/icons-material/FolderOpen';
import DeleteIcon from '@mui/icons-material/Delete';

const DeleteProject = (props) => {
    const { deleteProject, actived } = props;

    if (actived) {
        return (
            <span className="lineButtons"><Button onClick={deleteProject} variant="outlined" color="error" startIcon={<DeleteIcon />}>Delete</Button></span>
        );
    }
}

const CreateProjectButton = (props) => {
    const { createProject, actived } = props;

    if (actived) {
        return (
            <span className="lineButtons"><Button variant="outlined" startIcon={<AddIcon />} sx={{ color: '#005f6b', }} onClick={createProject}>Novo</Button></span>
        );
    }
}

const CreateProject = (props) => {
    const { close, open, userid } = props;

    const [loading, setLoading] = React.useState(true);
    const [loaded, setLoaded] = React.useState(false);
    const [alert, setAlert] = React.useState('');

    const [name, setName] = React.useState(';')
    const [networkId, setNetworkId] = React.useState(1);

    const [networks, setNetworks] = React.useState([]);

    const handleClose = () => { close(''); }
    const handleNetwork = (event) => { setNetworkId(event.target.value); }

    const post = () => {
        setLoading(true);

        const data = { Name: name, NetId: networkId, Owner: userid };
        console.log(data);
        axios.post('project/create', data)
            .then((response) => {
                const info = response["data"];
                console.log(response);
                if (info["Id"] < 0) {
                    setAlert(info["Message"]);
                    setLoading(false);
                }
                else {
                    close(info);
                    setLoading(false);
                }
            });
    }

    const get = () => {
        axios.get('networks/push').then((response) => {
            setNetworks(response["data"]);
            console.log(response["data"]);
            setLoading(false);
            setLoaded(true);
        });
    }

    if (open) {
        if (!loaded) { get(); }
        return (loading ? <CircularProgress sx={{ marginLeft: '50vw', marginTop: '50vh', }} /> :
            <Dialog onClose={handleClose} open={open}>
                <Collapse in={alert != ''} sx={{ heigth: "48px" }}>
                    <Alert
                        size="small"
                        sx={{ width: "92%" }}
                        severity="error"
                        action={
                            <IconButton
                                aria-label="close"
                                color="inherit"
                                size="small"
                                onClick={() => {
                                    setAlert('');
                                }}
                            >
                                <CloseIcon />
                            </IconButton>
                        }
                    >
                        {alert}
                    </Alert>
                </Collapse>
                <DialogTitle sx={{ padding: '12px 16px', marginLeft: '16px' }}><span className="title">Criar Projeto</span></DialogTitle>
                <FormControl required sx={{ marginLeft: '4%', marginBottom: '16px', width: '92%' }} variant="outlined">
                    <OutlinedInput
                        id="name"
                        type="text"
                        startAdornment={
                            <InputAdornment position="start">
                                <AccountTreeIcon />
                            </InputAdornment>
                        }
                        label="Account"
                        onChange={e => setName(e.target.value)}
                    />
                    <InputLabel htmlFor="name">Nome</InputLabel>
                </FormControl>
                <FormControl required sx={{ marginLeft: '4%', marginBottom: '16px', width: '92%' }} variant="outlined">
                    <InputLabel id="network">Rede</InputLabel>
                    <Select labelId="network" id="network-select" value={networkId} label="Rede" onChange={handleNetwork}>
                        {
                            networks.map(net =>
                                <MenuItem value={net["Id"]}> {net["Name"]} </MenuItem>
                            )
                        }
                    </Select>
                </FormControl>

                <Button startIcon={<AddIcon />}  onClick={post} sx={{ marginLeft: '40%', width: '20%', marginBottom: '16px' }} variant="outlined">Criar</Button>
            </Dialog>
        );
    }
}

const Project = (props) => {
    const { userid } = props;

    const [project, setProject] = React.useState([0, '', 0, '', '', undefined, true, false]);

    const [createProject, setCreateProject] = React.useState(false);


    const openCreateProject = () => { setCreateProject(true); }

    const deleteProject = () => { }

    const createProjectResult = (result) => {
        if (result == '') { setCreateProject(false); }
        else {
            setCreateProject(false);

            console.log(result);
        }
    }

    return ( 
        <div className="wh100 flex grid">
            <Grid container spacing={2}>
                <Grid item xs={5}>
                    <Box className="gridItem" sx={{ width: '92%', heigth: '96%', padding: '2% 4%' }}>
                        <div className="line borderBottom"><div className="iconAdjust"><AccountTreeIcon sx={{ color: '#005f6b', }} /></div><span className="info">Projeto:</span>&nbsp;{project[1]}</div>
                        <div className="line"><div className="iconAdjust"><PersonIcon sx={{ color: '#005f6b', }} /></div><span className="info">Responsável: </span>&nbsp;{project[3]}</div>
                        <div className="line"><div className="iconAdjust"><SpokeIcon sx={{ color: '#005f6b', }} /></div><span className="info">Rede usada:</span>&nbsp;{project[4]}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div className="iconAdjust"><ShowChartIcon sx={{ color: '#005f6b', }} /></div><span className="info">Accuracy:</span>&nbsp;{project[5] * 100}%</div>
                        <div className="line">
                            <CreateProjectButton createProject={openCreateProject} actived={project[6]}/>
                            <span className="lineButtons"><Button variant="outlined" startIcon={<FolderOpenIcon />} sx={{ color: '#005f6b', }}>Carregar</Button></span>
                            <DeleteProject deleteProject={deleteProject} actived={project[7]}/>
                        </div>
                    </Box>
                    <Box sx={{ width: '100%', heigth: '100%', backgroundColor: 'yellow', }}>Teste</Box>
                </Grid>
                <Grid item xs={7}>
                    <Box sx={{ width: '100%', heigth: '100%', backgroundColor: 'yellow', }}>Teste</Box>
                </Grid>
            </Grid>
            <CreateProject close={createProjectResult} open={createProject} userid={userid} />
        </div>
    );
}

export default Project;