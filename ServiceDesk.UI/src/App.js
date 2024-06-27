import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import { Login } from './components/Login';
import { Register } from './components/Register';
import Dashboard from './components/Dashboard';
import Tickets from "./components/Tickets";
import Users, { fetchUsers } from "./components/Users";
import User, { fetchUser } from "./components/User";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Dashboard />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  },
  {
    path: "/tickets",
    element: <Tickets />,
  },
  {
    path: "/users",
    element: <Users />,
    loader: fetchUsers
  },
  {
    path: "/users/:id",
    element: <User />,
    loader: fetchUser
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
