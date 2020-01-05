import * as React from 'react';
import './navbar.scss';
import { APPLICATION_NAME } from '../shared';
import { push } from 'connected-react-router';
import { Link } from 'react-router-dom';

export const Navbar = () => {
    return <div className="navbar__component">
        <div className="__container">
            <Link to="/">
                <span>{APPLICATION_NAME}</span>
            </Link>
        </div>
    </div>;
}