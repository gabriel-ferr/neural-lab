import React, { Component, useState } from 'react';
import axios from 'axios';

import { Button } from '@mui/material';

const FileUpload = () => {
    const [file, setFile] = useState();
    const [fileName, setFileName] = useState();

    const saveFile = (e) => {

        console.log(e.target.files[0]);
        setFile(e.target.files[0]);
        setFileName(e.target.files[0].name);
    };

    const uploadFile = async (e) => {
        const formData = new FormData()
        formData.append('File', new Blob([file]));

        const data = { UserID: 0, ProjectID: 0, File: new Blob([file]) }
        
        try {
            const res = await axios.post("data/import-quantitative", formData);
        } catch (ex) { console.log(ex); }
    }
    return (
        <div>
            <input type="file" onChange={saveFile} />
            <Button onClick={uploadFile}>Upload</Button>
        </div>
    );
}

export default FileUpload;