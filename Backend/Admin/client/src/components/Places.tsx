import {
  Container,
  Paper,
  Grid,
  TextField,
  Button,
  Box,
  Divider,
  InputAdornment,
} from "@material-ui/core";
import { Search } from "@material-ui/icons";

type PlaceCardProps = {
  id: string;
  name: string;
  imageUrl: string;
  description: string;
  points: number;
};

function PlaceCard({
  id,
  name,
  imageUrl,
  description,
  points,
}: PlaceCardProps) {}

export default function Places({}) {
  return (
    <Box pt={1}>
      <Container>
        <Paper elevation={1}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={8}>
              <Box ml={1}>
                <Button>New Place</Button>
                <Button>Import JSON</Button>
              </Box>
              <Divider orientation="vertical" flexItem />
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
      </Container>
    </Box>
  );
}
