import * as React from 'react';
import './navbar.scss';
import { APPLICATION_NAME } from '../shared';

export const Navbar = () => <div className="navbar__component">
    <div className="__container">{APPLICATION_NAME}</div>
</div>