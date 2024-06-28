import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import { Login } from './components/Login';
import { Register } from './components/Register';
import Tickets, { fetchTickets } from "./components/Tickets";
import Users, { fetchUsers } from "./components/Users";
import User, { fetchUser } from "./components/User";
import ErrorBoundary from "./components/ErrorBoundary";
import Ticket, { fetchTicket } from "./components/Ticket";
import Root from "./components/Root";
import Dashboard from "./components/Dashboard";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    errorElement: <ErrorBoundary />,
    children: [
      {
        path: "",
        element: <Dashboard />,
        errorElement: <ErrorBoundary />,
      },
      {
        path: "/login",
        element: <Login />,
        errorElement: <ErrorBoundary />,
      },
      {
        path: "/register",
        element: <Register />,
        errorElement: <ErrorBoundary />,
      },
      {
        path: "/tickets",
        element: <Tickets />,
        errorElement: <ErrorBoundary />,
        loader: fetchTickets,
      },
      {
        path: "/tickets/:id",
        element: <Ticket />,
        errorElement: <ErrorBoundary />,
        loader: fetchTicket,
      },
      {
        path: "/users",
        element: <Users />,
        errorElement: <ErrorBoundary />,
        loader: fetchUsers
      },
      {
        path: "/users/:id",
        element: <User />,
        errorElement: <ErrorBoundary />,
        loader: fetchUser
      },
    ]
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
