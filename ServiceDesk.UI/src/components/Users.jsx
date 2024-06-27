import { Link, useLoaderData } from 'react-router-dom';

export async function fetchUsers() {
    const res = await fetch('http://localhost:5406/api/User');

    if (!res.ok) throw new Error('Failed to load users');

    const users = await res.json();

    return { users };
};

export default function Users() {
    const { users } = useLoaderData();

    return (
        <table>
            <thead>
                <tr>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Email</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user => (
                    <tr key={user.id}>
                        <td>
                            {user.firstName}
                        </td>
                        <td>
                            {user.lastName}
                        </td>
                        <td>
                            {user.email}
                        </td>
                        <td>
                            <Link to={`/users/` + user.id}>
                                View
                            </Link>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}
