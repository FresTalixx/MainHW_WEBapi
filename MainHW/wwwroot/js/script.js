


const registerForm = document.getElementById('registerForm');



registerForm.addEventListener('submit', async (event) => {
    event.preventDefault();

    const formData = new FormData(registerForm);

    const registerRequest = {
        name: formData.get('name'),
        email: formData.get('email'),
        password: formData.get('password')
    }

    const errorMessageElement = document.getElementById('register-errors');
    fetch('/api/User/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(registerRequest)
    })
        .then(response => {
            console.log('Response status:', response.status);
            return response.json()
                .then(response => {
                    console.log('Response data:', response);
                    return response;
                })
                .catch (error => {
                    console.error('Error parsing response:', error, data);
                });
        })
        .then(data => {
            if (data != null) {
                errorMessageElement.textContent = 'Registration successful!';
                localStorage.setItem('token', data.token);
                console.log('Token stored in localStorage:', data.token);
                console.log('Token retrieved from localStorage:', localStorage.getItem('token'));
            } else {

                /*
    {"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":
    {"Name":["Name must be at least 3 characters long."],"Email":["Invalid email format."],"Password":["Password must be at least 6 characters long."]},"traceId":"00-47380a69b0ea876d3d2c1092effb9c80-78eed854a1fb873a-00"}
    */

                const errors = data.errors;

                const errorMessages = [];
                for (const key in errors) {
                    if (errors.hasOwnProperty(key)) {
                        errorMessages.push(...errors[key]);
                    }
                }

                errorMessageElement.textContent = errorMessages.join(', ');
            }
        })
        .catch(error => {
            errorMessageElement.textContent = 'An error occurred: ' + error.message;
        });



});



const getAllMessagesButton = document.getElementById("getAllMessagesBtn");

getAllMessagesButton.addEventListener("click", async () => {
    await fetch("/messages", {
        method: "GET",
        headers: {
            "X-My-Authorization": localStorage.getItem('token'),
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            return response.json()
                .then(data => {
                    console.log('Response data:', data);
                    return data;
                })
                .catch(error => {
                    console.error('Error parsing response:', error);
                })
        })
        .then(data => {
            console.log(data);
        const messagesTable = document.getElementById('messagesTable');
        const tbody = messagesTable.getElementsByTagName('tbody')[0];
        tbody.innerHTML = '';
        data.forEach(message => {
            const row = tbody.insertRow();
            const idCell = row.insertCell(0);
            const fromCell = row.insertCell(1);
            const textCell = row.insertCell(2);
            const createdAtCell = row.insertCell(3);
            idCell.textContent = message.id;
            fromCell.textContent = message.from;
            textCell.textContent = message.text;
            createdAtCell.textContent = message.createdAt;
        });
    })
    .catch(error => {
        console.error('Error fetching messages:', error);
        const errorMessageElement = document.getElementById('error-message-get-messages');
        errorMessageElement.textContent = 'Error fetching messages: ' + error.message + " JSON: " + data;
    });;
});