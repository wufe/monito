import * as React from 'react';
import './hero.scss';
import FloorAnimation from '@wufe/floor-animation';

export const Hero = () => <div className="hero__component">
    {/* <FloorAnimation yaw={2.9} color="#3b4252" /> */}
    <div className="__overlay"></div>
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