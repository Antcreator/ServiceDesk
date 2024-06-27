import { useLoaderData } from 'react-router-dom';

export async function fetchUser({ params }) {
    const res = await fetch('http://localhost:5406/api/User/' + params.id);

    if (!res.ok) throw new Error('Failed to load user');

    const user = await res.json();

    return { user };
};

export default function User() {
    const { user } = useLoaderData();

    return (
        <>
            <section>
                <h2>Contact Info</h2>
                <p>
                    Name - {user.firstName} {user.lastName}
                </p>
                <p>
                    Email - {user.email}
                </p>
                <p>
                    Role - {user.role}
                </p>
            </section>
            {user.issues.length && (
                <section>
                    <h2>Tickets</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>Subject</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {user.issues.map(ticket => (
                                <tr key={ticket.id}>
                                    <td>
                                        {ticket.subject}
                                    </td>
                                    <td>
                                        <a href="">View</a>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </section>
            )}
        </>
    );
}
