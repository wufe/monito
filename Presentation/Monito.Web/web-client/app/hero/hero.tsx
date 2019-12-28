import * as React from 'react';
import './hero.scss';
import FloorAnimation from '@wufe/floor-animation';

export const Hero = () => <div className="hero__component">
    <FloorAnimation yaw={2.4} color="#000" />
    <div className="__content">
        <div className="__title">
            Free link status checker.
        </div>
        <div className="__subtitle">
            Provides stats about your batch of links.
        </div>
        <div className="__subtitle">
            Does not require signup.
        </div>
    </div>
</div>