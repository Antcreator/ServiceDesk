import { useLoaderData } from 'react-router-dom';

export async function fetchTicket({ params }) {
    const res = await fetch('http://localhost:5403/api/Ticket/' + params.id);

    if (!res.ok) throw new Error('Failed to load ticket');

    const ticket = await res.json();

    return { ticket };
};

export default function Ticket() {
    const { ticket } = useLoaderData();
    const downloadDocument = (document) => {
        window.open(`http://localhost:5401/api/Document/${document.id}/download`, '_blank');
    };

    return (
        <>
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
            <section>
                <h2>Documents</h2>
                <table>
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Download</th>
                        </tr>
                    </thead>
                    <tbody>
                        {ticket.documents.map(document => (
                            <tr key={document.id}>
                                <td>{document.title}</td>
                                <td>
                                    <button onClick={downloadDocument}>Download</button>
                                    <button>Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </section>
        </>
    );
}
