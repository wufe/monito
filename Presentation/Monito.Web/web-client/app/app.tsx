import * as React from 'react';
import './app.scss';
import { Navbar } from '~/app/navbar/navbar';
import { Switch, Route } from 'react-router-dom';
import { Hero } from '~/app/hero/hero';
import { useDispatch } from 'react-redux';
import { LazyBuilder } from '~/app/shared';
import { LoadingBarContainer } from '~/app/loading/loading-bar-container';
import background from '~/assets/3254307.png';
import background2 from '~/assets/3323904.png';

console.log(background);

const LazyJobPageBuilder = LazyBuilder(import('~/app/pages/job/jobpage'));
const LazyHomePageBuilder = LazyBuilder(import('~/app/pages/home/homepage'));

export const App = () => {

    const dispatch = useDispatch();
    const JobPage = LazyJobPageBuilder(dispatch);
    const HomePage = LazyHomePageBuilder(dispatch);

    return <div className="app__component">
        <div className="__background">
            <img src={background2} />
        </div>
        <LoadingBarContainer />
        <Navbar />
        <Switch>
            <Route exact path="/">
                <Hero />
                <React.Suspense fallback="">
                    <HomePage />
                </React.Suspense>
            </Route>
            <Route path="/job/:userUUID/:jobUUID">
                <React.Suspense fallback="">
                    <JobPage />
                </React.Suspense>
            </Route>
        </Switch>
    </div>;
}