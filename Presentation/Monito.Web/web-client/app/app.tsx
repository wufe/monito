import * as React from 'react';
import './app.scss';
import { Navbar } from '~/app/navbar/navbar';
import { Switch, Route } from 'react-router-dom';
import { Hero } from '~/app/hero/hero';

const JobPage = React.lazy(() => import('~/app/pages/job/jobpage'));
const HomePage = React.lazy(() => import('~/app/pages/home/homepage'));

export const App = () => <div className="app__component">
    <Navbar />
    <Hero />
    <Switch>
        <Route exact path="/">
            <React.Suspense fallback="Loading..">
                <HomePage />
            </React.Suspense>
        </Route>
        <Route path="/job/:jobID">
            <React.Suspense fallback="Loading..">
                <JobPage />
            </React.Suspense>
        </Route>
    </Switch>
</div>