import * as React from 'react';
import './tospage.scss';

export const TOSPage = () => <div className="tos-page__component">
    <div className="__container">
        <h1>Terms of Service</h1>
        <p>
            <h2>Logs</h2>
            <span>
                User logs and tasks results are deleted after a finite period of time.
            </span>
        </p>
        <p>
            <h2>Restriction</h2>
            <span>The service acts as a URI status checker and cannot be used for purposes different from the ones it was created for.</span>
            <br />
            <span>Forwarding request to the same host may result in an increasing delay between requests.</span>
            <br />
            <span>The service does not guarantee that the URIs provided will be checked in the same order.</span>
        </p>
        <p>
            <h2>Proxy</h2>
            <span>In order to protect the service from inappropriate usage, the service acts as a proxy, forwarding the client's IP address through X-FORWARDED-FOR and X-REAL-IP HTTP headers.</span>
            <br />
            <span>The user's IP address is used for this purpose only and it is not related to information that would personally identify you.</span>
        </p>
        <p>
            <h2>Policies</h2>
            <span>We reserve the right to make changes to the website's policies at any time.</span>
        </p>
    </div>
</div>

export default TOSPage;