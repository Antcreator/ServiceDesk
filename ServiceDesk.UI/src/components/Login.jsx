export function Login() {
    const handleSubmit = (e) => {
        e.preventDefault();

        const form = e.target;
        const data = new FormData(form);
        const payload = Object.fromEntries(data.entries());

        fetch('http://localhost:5402/api/Auth/Login', { 
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
            <input type="email" name="email" placeholder="Email" />
            <input type="password" name="password" placeholder="Password" />
            <button type="submit">Login</button>
        </form>
    );
}
