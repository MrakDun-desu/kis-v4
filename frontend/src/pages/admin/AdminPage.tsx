import { AppBar, MenuItem, MenuList } from "@mui/material";
import { Outlet, useLocation, useNavigate } from "react-router-dom";

interface Link {
  label: string;
  url: string;
}
const links: Link[] = [
  {
    label: "Skladové položky",
    url: "store-items",
  },
  {
    label: "Sklady",
    url: "stores",
  },
  {
    label: "Skladové transakce",
    url: "store-transactions",
  },
  {
    label: "Prodejní položky",
    url: "sale-items",
  },
  {
    label: "Modifikátory",
    url: "modifiers",
  },
  {
    label: "Prodejní transakce",
    url: "sale-transactions",
  },
  {
    label: "Kasy",
    url: "cashboxes",
  },
  {
    label: "Typy kegů",
    url: "container-templates",
  },
  {
    label: "Kegy",
    url: "containers",
  },
  {
    label: "Pípy",
    url: "pipes",
  },
  {
    label: "Uživatelé",
    url: "users",
  },
  {
    label: "Layouty",
    url: "layouts",
  },
  {
    label: "Slevy",
    url: "discounts",
  },
];

export const AdminPage = () => {
  const { pathname } = useLocation();
  const navigate = useNavigate();

  const path_parts = pathname.split("/");
  const path_end = path_parts[path_parts.length];

  const open_link = (path: string) => navigate(`/admin/${path}`);
  return (
    <div className="admin-container">
      <AppBar position="static"></AppBar>
      <div className="menu">
        <MenuList>
          {links.map((val, id) => {
            const disabled = path_end == val.url;
            return (
              <MenuItem
                disabled={disabled}
                key={id}
                onClick={() => open_link(val.url)}
              >
                {val.label}
              </MenuItem>
            );
          })}
        </MenuList>
      </div>
      <Outlet />
    </div>
  );
};
