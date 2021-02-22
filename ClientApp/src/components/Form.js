import React, { Component } from 'react';

class Form extends Component {
    constructor(props) {
        super(props);

        this.formSubmit = this.formSubmit.bind(this);
    }

    async formSubmit(event) {
        event.preventDefault();
        const form = event.target;
        const id = form.elements["id"].value;
        const dateRegistration = form.elements["dateRegistration"].value;
        const dateLastActivity = form.elements["dateLastActivity"].value;

        if (new Date(dateRegistration) > new Date(dateLastActivity)) {
            alert("Регистрация не может быть позже последней активности");
            return;
        }

        if (!Number.isInteger(parseInt(id))) {
            alert("ID - не целое число");
            return;
        }
        else {
            if (parseInt(id) <= 0) {
                alert("ID - не верный формат");
                return;
            }
        }

        await fetch('/api/Users', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: id, dateRegistration: dateRegistration, dateLastActivity: dateLastActivity })
        })
        .catch(error => console.log(error));

        this.props.updAudit();
        form.reset();
    }

    render() {
        return (
            <form class="col-12" onSubmit={this.formSubmit}>
                <span class="col-12 title_section">New item</span>

                <div class="col-12 d-flex">
                    <div class="col-2 title_section text-center">UserID</div>
                    <div class="col-4 offset-md-2 title_section text-center">Date Registration</div>
                    <div class="col-4 title_section text-center">Date Last Activity</div>
                </div>
                <div class="col-12 d-flex py-3">
                    <div class="col-2">
                        <input class="text-center" type="text" id='id' name="id" placeholder="1" required autoComplete="false" />
                    </div>
                    <div class="col-4 offset-md-2">
                        <input class="text-center" type="date" id='dateRegistration' name="dateRegistration" required autoComplete="false" />
                    </div>
                    <div class="col-4">
                        <input class="text-center" type="date" id='dateLastActivity' name="dateLastActivity" required autoComplete="false" />
                    </div>
                </div>
                <div class="col-12 d-flex justify-content-center mt-5">
                    <button class="btn btn-action px-5" type="submit">
                        Save
                    </button>
                </div>
            </form>
        )
    }

}

export default Form;