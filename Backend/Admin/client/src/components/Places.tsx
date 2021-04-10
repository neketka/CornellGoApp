import {
  Container,
  Paper,
  Grid,
  TextField,
  Button,
  Box,
  Divider,
  CardHeader,
  CardMedia,
  CardContent,
  CardActions,
  InputAdornment,
  Typography,
  IconButton,
  Card,
  makeStyles,
  Dialog,
  DialogContentText,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@material-ui/core";
import { Delete, Edit, Search } from "@material-ui/icons";

type PlaceCardProps = {
  id: string;
  name: string;
  imageUrl: string;
  description: string;
  points: number;
};

const useStyles = makeStyles((theme) => ({
  media: {
    paddingTop: "40%",
  },
}));
    
function PlaceCard({
  id,
  name,
  imageUrl,
  description,
  points,
}: PlaceCardProps) {
  const classes = useStyles();
  return (
    <Card>
      <CardHeader
        action={
          <IconButton>
            <Edit />
          </IconButton>
        }
        title={name}
        subheader={points + (points === 1 ? " point" : " points")}
      />
      <CardMedia image={imageUrl} className={classes.media} />
      <CardContent>
        <Typography variant="body2" color="textSecondary" component="p">
          {description}
        </Typography>
      </CardContent>
      <CardActions disableSpacing>
        <IconButton>
          <Delete />
        </IconButton>
      </CardActions>
    </Card>
  );
}

type EditorDialogProps = {
  close: () => void;
  opened: boolean;
};

export function EditorDialog({ close, opened }: EditorDialogProps) {
  const finish = () => {
    close();
  };

  return (
    <Dialog open={opened} onClose={close} scroll="body">
      <DialogTitle id="scroll-dialog-title">Subscribe</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Cras mattis consectetur purus sit amet fermentum. Cras justo odio,
          dapibus ac facilisis in, egestas eget quam. Morbi leo risus, porta ac
          consectetur ac, vestibulum at eros. Praesent commodo cursus magna, vel
          scelerisque nisl consectetur et. Vivamus sagittis lacus vel augue
          laoreet rutrum faucibus dolor auctor. Aenean lacinia bibendum nulla
          sed consectetur. Praesent commodo cursus magna, vel scelerisque nisl
          consectetur et. Donec sed odio dui. Donec ullamcorper nulla non metus
          auctor fringilla. Cras mattis consectetur purus sit amet fermentum.
          Cras justo odio, dapibus ac facilisis in, egestas eget quam. Morbi leo
          risus, porta ac consectetur ac, vestibulum at eros. Praesent commodo
          cursus magna, vel scelerisque nisl consectetur et. Vivamus sagittis
          lacus vel augue laoreet rutrum faucibus dolor auctor. Aenean lacinia
          bibendum nulla sed consectetur. Praesent commodo cursus magna, vel
          scelerisque nisl consectetur et. Donec sed odio dui. Donec ullamcorper
          nulla non metus auctor fringilla. Cras mattis consectetur purus sit
          amet fermentum. Cras justo odio, dapibus ac facilisis in, egestas eget
          quam. Morbi leo risus, porta ac consectetur ac, vestibulum at eros.
          Praesent commodo cursus magna, vel scelerisque nisl consectetur et.
          Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor.
          Aenean lacinia bibendum nulla sed consectetur. Praesent commodo cursus
          magna, vel scelerisque nisl consectetur et. Donec sed odio dui. Donec
          ullamcorper nulla non metus auctor fringilla. Cras mattis consectetur
          purus sit amet fermentum. Cras justo odio, dapibus ac facilisis in,
          egestas eget quam. Morbi leo risus, porta ac consectetur ac,
          vestibulum at eros. Praesent commodo cursus magna, vel scelerisque
          nisl consectetur et. Vivamus sagittis lacus vel augue laoreet rutrum
          faucibus dolor auctor. Aenean lacinia bibendum nulla sed consectetur.
          Praesent commodo cursus magna, vel scelerisque nisl consectetur et.
          Donec sed odio dui. Donec ullamcorper nulla non metus auctor
          fringilla. Cras mattis consectetur purus sit amet fermentum. Cras
          justo odio, dapibus ac facilisis in, egestas eget quam. Morbi leo
          risus, porta ac consectetur ac, vestibulum at eros. Praesent commodo
          cursus magna, vel scelerisque nisl consectetur et. Vivamus sagittis
          lacus vel augue laoreet rutrum faucibus dolor auctor. Aenean lacinia
          bibendum nulla sed consectetur. Praesent commodo cursus magna, vel
          scelerisque nisl consectetur et. Donec sed odio dui. Donec ullamcorper
          nulla non metus auctor fringilla. Cras mattis consectetur purus sit
          amet fermentum. Cras justo odio, dapibus ac facilisis in, egestas eget
          quam. Morbi leo risus, porta ac consectetur ac, vestibulum at eros.
          Praesent commodo cursus magna, vel scelerisque nisl consectetur et.
          Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor.
          Aenean lacinia bibendum nulla sed consectetur. Praesent commodo cursus
          magna, vel scelerisque nisl consectetur et. Donec sed odio dui. Donec
          ullamcorper nulla non metus auctor fringilla.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={close} color="primary">
          Cancel
        </Button>
        <Button onClick={finish} color="primary">
          Save
        </Button>
      </DialogActions>
    </Dialog>
  );
}

export default function Places({}) {
  return (
    <Grid container justify="center" alignContent="center">
      <Grid
        container
        style={{ position: "sticky", top: 90, opacity: 0.95, zIndex: 100 }}
      >
        <Grid item xs={1} />
        <Grid item xs={10}>
          <Paper elevation={1}>
            <Grid container spacing={2} alignItems="center">
              <Grid item xs={8}>
                <Box ml={1}>
                  <Button>New Place</Button>
                </Box>
              </Grid>
              <Grid item xs={4}>
                <Box mr={1}>
                  <TextField
                    id="input-with-icon-grid"
                    variant="outlined"
                    size="small"
                    fullWidth
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <Search />
                        </InputAdornment>
                      ),
                    }}
                  />
                </Box>
              </Grid>
            </Grid>
          </Paper>
        </Grid>
      </Grid>
      <Grid container direction="column" style={{ marginTop: 32 }}>
        <Grid container style={{ marginBottom: 16 }}>
          <Grid xs={3} item />
          <Grid xs={6} item>
            <PlaceCard
              id=""
              name="My place"
              imageUrl="https://www.publicdomainpictures.net/pictures/280000/velka/erfolg.jpg"
              description="abc"
              points={0}
            />
          </Grid>
        </Grid>
        <Grid container style={{ marginBottom: 16 }}>
          <Grid xs={3} item />
          <Grid xs={6} item>
            <PlaceCard
              id=""
              name="My place"
              imageUrl="https://www.publicdomainpictures.net/pictures/280000/velka/erfolg.jpg"
              description="abc"
              points={0}
            />
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
}
