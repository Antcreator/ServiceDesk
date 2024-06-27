import { useState, useEffect } from 'react';

export default function Users() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const fetchUsers = async () => {
        try {
            const res = await fetch('http://localhost:5406/api/User');

            if (!res.ok) throw new Error('Failed to load users');

            const users = await res.json();

            setUsers(users);
        } 
        catch (error) { setError(error.message); } 
        finally { setLoading(false); }
    };

    useEffect(() => { fetchUsers(); }, []);

    if (loading) return <div>Loading...</div>
    if (error) return <div>Oops! Something failed - {error}</div>

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
                            <a href="">View</a>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}
