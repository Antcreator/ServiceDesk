export function Register() {
    const handleSubmit = (e) => {
        e.preventDefault();

        const form = e.target;
        const data = new FormData(form);
        const payload = Object.fromEntries(data.entries().map(([key, val]) => [
            key, key === 'role' ? parseInt(val) : val
        ]));

        fetch('http://localhost:5406/api/User', { 
            method: form.method, 
            body: JSON.stringify(payload),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
    };

    return (
        <form method="post" onSubmit={handleSubmit}>
            <input type="text" name="firstName" placeholder="First Name" />
            <input type="text" name="lastName" placeholder="Last Name" />
            <input type="email" name="email" placeholder="Email" />
            <select name="role">
                <option value="0">Admin</option>
                <option value="1">Support</option>
                <option value="2">Staff</option>
            </select>
            <input type="password" name="password" placeholder="Password" />
            <button type="submit">Register</button>
        </form>
    );
}
