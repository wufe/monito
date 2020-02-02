import * as React from 'react';
import './app.scss';
import { Navbar } from '~/app/navbar/navbar';
import { Switch, Route } from 'react-router-dom';
import { Hero } from '~/app/hero/hero';
import { useDispatch } from 'react-redux';
import { LazyBuilder } from '~/app/shared';
import { LoadingBarContainer } from '~/app/loading/loading-bar-container';
import LargeBackground from '~/assets/3323904.png';

const LazyJobPageBuilder = LazyBuilder(import('~/app/pages/job/jobpage'));
const LazyHomePageBuilder = LazyBuilder(import('~/app/pages/home/homepage'));
const LazyTOSPageBuilder = LazyBuilder(import('~/app/pages/tos/tospage'));

export const App = () => {

    const dispatch = useDispatch();
    const JobPage = LazyJobPageBuilder(dispatch);
    const HomePage = LazyHomePageBuilder(dispatch);
    const TOSPage = LazyTOSPageBuilder(dispatch);

    return <div className="app__component">
        <div className="__background">
            <img src={LargeBackground} />
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
            <Route path="/tos">
                <React.Suspense fallback="">
                    <TOSPage />
                </React.Suspense>
            </Route>
        </Switch>
    </div>;
}