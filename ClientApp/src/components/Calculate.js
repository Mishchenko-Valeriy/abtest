import React, { Component } from 'react';

class Calculate extends Component {
    constructor(props) {
        super(props);

        this.state = {
            info: []
        };

        this.getInfo = this.getInfo.bind(this);
    }

    async getInfo() {
        const res = await fetch('/api/Calculate');
        const response = await res.json();
        this.setState({ info: response });
    }

    render() {
        return (
            <div>
                <div class="col-12 d-flex justify-content-left mt-5">
                    <button class="btn btn-action px-5" onClick={this.getInfo}>
                        Calculate
                    </button>
                </div>
                {this.state.info.x != undefined &&
                
                    <div class="col-12 title_section">
                        Rolling Retention {this.state.info.x} day = <b>{this.state.info.rollingRetention} %</b>

                        <br/>

                        Count Date Last Activity = {this.state.info.countDateLastActivity}

                        <br />

                        Count Date Last Registration = {this.state.info.countDateRegistration}

                        <br />

                        Executing an SQL request: {this.state.info.timeSQL} ms

                        <br />

                        Execution of the calculation part: {this.state.info.timeMath} ms
                    </div>
                }
            </div>
        )
    }

}

export default Calculate;