import {
  AppBar,
  Box,
  Button,
  Divider,
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Typography,
} from "@mui/material";
import {
  AccountCircle,
  Build,
  Discount,
  GridView,
  Inventory,
  Liquor,
  MoveDown,
  OilBarrel,
  PointOfSale,
  Receipt,
  ShoppingBag,
  Storefront,
  Store,
  WaterDrop,
} from "@mui/icons-material";
import { type ReactElement } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../auth/AuthContext";

const drawerWidth = 240;

interface Link {
  label: string;
  url: string;
  icon?: ReactElement<any, any>;
}
const links: Link[][] = [
  [
    {
      label: "Skladové položky",
      url: "store-items",
      icon: <Inventory />,
    },
    {
      label: "Skladové transakce",
      url: "store-transactions",
      icon: <MoveDown />,
    },
    {
      label: "Sklady",
      url: "stores",
      icon: <Store />,
    },
  ],

  [
    {
      label: "Prodejní položky",
      url: "sale-items",
      icon: <ShoppingBag />,
    },
    {
      label: "Prodejní transakce",
      url: "sale-transactions",
      icon: <Receipt />,
    },
    {
      label: "Modifikátory",
      url: "modifiers",
      icon: <Build />,
    },
    {
      label: "Layouty",
      url: "layouts",
      icon: <GridView />,
    },
    {
      label: "Slevy",
      url: "discounts",
      icon: <Discount />,
    },
  ],

  [
    {
      label: "Typy kegů",
      url: "container-templates",
      icon: <Liquor />,
    },
    {
      label: "Kegy",
      url: "containers",
      icon: <OilBarrel />,
    },
    {
      label: "Pípy",
      url: "taps",
      icon: <WaterDrop />,
    },
  ],

  [
    {
      label: "Kasy",
      url: "cashboxes",
      icon: <PointOfSale />,
    },
    {
      label: "Uživatelé",
      url: "users",
      icon: <AccountCircle />,
    },
    {
      label: "Operátor",
      url: "/pos",
      icon: <Storefront />,
    },
  ],
];

export const AdminPage = () => {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const auth = useAuth();

  const path_parts = pathname.split("/");
  const path_end = path_parts[path_parts.length - 1];

  return (
    <>
      <AppBar
        position="fixed"
        sx={{
          width: `calc(100% - ${drawerWidth}px)`,
          marginLeft: drawerWidth,
        }}
      >
        <Toolbar>
          <Typography variant="h6" noWrap component="h1" flexGrow={1}>
            Kachní informační systém
          </Typography>
          {auth.userClaims && (
            <span>
              uživatel:{" "}
              {auth.userClaims.find((claim: any) => claim["type"] === "name")
                ?.value ?? "Neznámý"}
            </span>
          )}
          <Button onClick={auth.signOut}>Odhlásit se</Button>
        </Toolbar>
      </AppBar>
      <Drawer
        variant="permanent"
        anchor="left"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          "& .MuiDrawer-paper": {
            width: drawerWidth,
            boxSizing: "border-box",
          },
        }}
      >
        <List>
          {links.map((arr, id1) => {
            var elements = arr.map((val, id2) => {
              const disabled = path_end == val.url;
              return (
                <ListItem disablePadding key={`${id1}${id2}`}>
                  <ListItemButton
                    disabled={disabled}
                    onClick={() => navigate(val.url)}
                  >
                    <ListItemIcon>{val.icon}</ListItemIcon>
                    <ListItemText primary={val.label} />
                  </ListItemButton>
                </ListItem>
              );
            });

            return [...elements, <Divider key="divider" />];
          })}
        </List>
      </Drawer>
      <Box marginTop={10} marginLeft={`${drawerWidth + 20}px`} marginRight={2}>
        <Outlet />
      </Box>
    </>
  );
};
