import { setSessionToken } from "../utils/UserSession";
import http from "../utils/HttpClient";
import { useNavigate } from "react-router-dom";

export function Login() {
    const navigate = useNavigate();
    const handleSubmit = async (e) => {
        e.preventDefault();

        const form = e.target;
        const data = Object.fromEntries(new FormData(form).entries());
        const { data: token } = await http.post('http://localhost:5402/api/Auth/Login', data);

        setSessionToken(token);
        navigate('/tickets');
    };

    return (
        <form method="post" onSubmit={handleSubmit}>
            <input type="email" name="email" placeholder="Email" />
            <input type="password" name="password" placeholder="Password" />
            <button type="submit">Login</button>
        </form>
    );
}
