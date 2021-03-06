import {
  AppBar,
  Toolbar,
  Typography,
  CssBaseline,
  TextField,
  Grid,
  Drawer,
  makeStyles,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
  Paper,
  Container,
  Button,
} from "@material-ui/core";

import {
  Group,
  Home,
  LocationOn,
  PeopleRounded,
  Person,
  Search,
  VerifiedUser,
} from "@material-ui/icons";

import Places from "./components/Places";
import { MemoryRouter, Switch, Route, useHistory } from "react-router-dom";
import AdminApproval from "./components/AdminApproval";

const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1,
  },
  drawer: {
    width: drawerWidth,
    flexShrink: 0,
  },
  drawerPaper: {
    width: drawerWidth,
  },
  drawerContainer: {
    overflow: "auto",
  },
  content: {
    flexGrow: 1,
    padding: theme.spacing(3),
  },
}));

type NavigationDrawerProps = {
  drawerClass: string;
  drawerPaper: string;
};

function NavigationDrawer({ drawerClass, drawerPaper }: NavigationDrawerProps) {
  const history = useHistory();
  return (
    <Drawer
      variant="permanent"
      className={drawerClass}
      classes={{ paper: drawerPaper }}
    >
      <Toolbar />
      <List>
        <ListItem button onClick={() => history.push("/")}>
          <ListItemIcon>
            <Home />
          </ListItemIcon>
          <ListItemText primary="Home" />
        </ListItem>
        <ListItem button onClick={() => history.push("/places")}>
          <ListItemIcon>
            <LocationOn />
          </ListItemIcon>
          <ListItemText primary="Places" />
        </ListItem>
        <ListItem button onClick={() => history.push("/admins")}>
          <ListItemIcon>
            <VerifiedUser />
          </ListItemIcon>
          <ListItemText primary="Admin Approval" />
        </ListItem>
        <ListItem button onClick={() => history.push("/users")}>
          <ListItemIcon>
            <PeopleRounded />
          </ListItemIcon>
          <ListItemText primary="Users" />
        </ListItem>
      </List>
    </Drawer>
  );
}

export default function App() {
  const classes = useStyles();

  return (
    <MemoryRouter>
      <AppBar position="fixed" className={classes.appBar}>
        <CssBaseline />
        <Toolbar>
          <Typography variant="subtitle1" color="inherit">
            CornellGO! Manager
          </Typography>
        </Toolbar>
      </AppBar>

      <NavigationDrawer
        drawerClass={classes.drawer}
        drawerPaper={classes.drawerPaper}
      />

      <div style={{ marginLeft: 240, marginTop: 84 }}>
        <Switch>
          <Route path="/" exact>
            <div>Home</div>
          </Route>
          <Route path="/places">
            <Places />
          </Route>
          <Route path="/admins">
            <AdminApproval />
          </Route>
          <Route path="/users">
            <div>Users</div>
          </Route>
        </Switch>
      </div>
    </MemoryRouter>
  );
}
