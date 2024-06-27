import { useState, useEffect } from 'react';

export default function Tickets() {
    const [tickets, setTickets] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const fetchTickets = async () => {
        try {
            const res = await fetch('http://localhost:5403/api/Ticket');

            if (!res.ok) throw new Error('Failed to load tickets');

            const tickets = await res.json();

            setTickets(tickets);
        } 
        catch (error) { setError(error.message); } 
        finally { setLoading(false); }
    };

    useEffect(() => { fetchTickets(); }, []);

    if (loading) return <div>Loading...</div>
    if (error) return <div>Oops! Something failed - {error}</div>

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
                            <a href="">View</a>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}
