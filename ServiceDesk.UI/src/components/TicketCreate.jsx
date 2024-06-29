import { getSessionUser } from "../utils/UserSession";
import http from "../utils/HttpClient";

export function TicketCreate() {
    const handleSubmit = (e) => {
        e.preventDefault();

        const form = e.target;
        const data = new FormData(form);
        const user = getSessionUser();

        data.append('reporterId', user.sub);
        http.post('http://localhost:5403/api/Ticket', data);
    };

    return (
        <form method="post" onSubmit={handleSubmit}>
            <input type="text" name="subject" placeholder="Subject" />
            <input type="text" name="description" placeholder="Description" />
            <input type="file" name="attachment" />
            <button type="submit">Submit</button>
        </form>
    );
}
