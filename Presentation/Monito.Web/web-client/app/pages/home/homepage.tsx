import * as React from 'react';
import './homepage.scss';
import { InfoHeader } from '~/app/pages/home/info-header/info-header';
import { InputFormContainer } from '~/app/pages/home/input-form/input-form-container';

export const HomePage = () => <div className="homepage__component">
    <InfoHeader />
    <InputFormContainer />
</div>;

export default HomePage;