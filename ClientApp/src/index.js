import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Form from "./components/Form";
import Calculate from "./components/Calculate";

class App extends Component {
    constructor(props) {
        super(props);

        this.state = {
            audit: []
        };

        this.getAudit = this.getAudit.bind(this);

        const DATE_OPTIONS = { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' };
    }

    componentDidMount() {
        this.getAudit();
    }

    async getAudit() {
        const res = await fetch('/api/Users');
        const response = await res.json();
        this.setState({ audit: response });
    }

    render() {
        return (
            <div class="row">
                <span class="col-12 title_section">Users</span>

                <Calculate/>

                <div class="col-12 d-flex px-md-4">
                    <div class="col-2 pl-0 title_tables">UserID</div>
                    <div class="col-4 pl-0 title_tables">Date Registration</div>
                    <div class="col-4 pl-0 title_tables">Date Last Activity</div>
                </div>
                <div class="accordion col-12 p-md-0" id="accordionCurrent">
                    <div class="card card-top border-0">

                        {this.state.audit.map((user, index) => {
                            return (
                                <div id="collapseOne" class="collapse show" aria-labelledby="headingOne"
                                    data-parent="#accordionCurrent">
                                    <div class="card-body d-flex">
                                        <span class="col-2 p-0">
                                            <p class="mb-0">
                                                {user.id}
                                            </p>
                                        </span>
                                        <span class="col-4 p-0">
                                            <p class="mb-0">
                                                {new Date(user.dateRegistration).toLocaleDateString('en-GB', this.DATE_OPTIONS)}
                                            </p>
                                        </span>
                                        <span class="col-4 p-0">
                                            <p class="mb-0">
                                                {new Date(user.dateLastActivity).toLocaleDateString('en-GB', this.DATE_OPTIONS)}
                                            </p>
                                        </span>
                                    </div>
                                </div>
                            );
                        })}


                    </div>

                    <Form updAudit={this.getAudit}/>

                </div>
            </div>
        );
    }
};

const rootElement = document.getElementById('root');
ReactDOM.render(<App />, rootElement);
