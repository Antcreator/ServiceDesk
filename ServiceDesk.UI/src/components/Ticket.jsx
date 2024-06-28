import { useLoaderData } from 'react-router-dom';

export async function fetchTicket({ params }) {
    const res = await fetch('http://localhost:5403/api/Ticket/' + params.id);

    if (!res.ok) throw new Error('Failed to load ticket');

    const ticket = await res.json();

    return { ticket };
};

export default function Ticket() {
    const { ticket } = useLoaderData();

    return (
        <section>
            <h2>Ticket Details</h2>
            <p>
                Subject - {ticket.subject}
            </p>
            <p>
                Description - {ticket.description}
            </p>
            <p>
                Reporter - {ticket.reporter.firstName} {ticket.reporter.lastName}
            </p>
        </section>
    );
}
