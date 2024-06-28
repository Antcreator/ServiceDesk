import http from '../utils/HttpClient';
import { Link, useLoaderData } from 'react-router-dom';

export async function fetchTickets() {
    const { data: tickets } = await http.get('http://localhost:5403/api/Ticket');

    return { tickets };
}

export default function Tickets() {
    const { tickets } = useLoaderData();

    return (
        <table>
            <thead>
                <tr>
                    <th>Subject</th>
                    <th>Reporter</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {tickets.map(ticket => (
                    <tr key={ticket.id}>
                        <td>
                            {ticket.subject}
                        </td>
                        <td>
                            {ticket.reporter.firstName} {ticket.reporter.lastName}
                        </td>
                        <td>
                            <Link to={`/tickets/` + ticket.id}>
                                View
                            </Link>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}
