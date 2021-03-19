import { AppBar, Toolbar, Typography } from '@material-ui/core';

import {
    BrowserRouter, Switch, Route
} from "react-router-dom";

export default function App() {
    return (
        <BrowserRouter>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="subtitle1" color="inherit">
                        CornellGO! Manager
                    </Typography>
                </Toolbar>
            </AppBar>
            
            <Switch>
                <Route path="/dashboard">
                </Route>
                <Route path="/">
                </Route>
            </Switch>
        </BrowserRouter>
    );
}
