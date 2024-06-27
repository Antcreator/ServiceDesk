import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import { Login } from './components/Login';
import { Register } from './components/Register';
import Dashboard from './components/Dashboard';
import Tickets from "./components/Tickets";

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
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
