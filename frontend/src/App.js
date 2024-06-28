import './styles/App.css';
import React, {useState, useEffect} from 'react';
import LinkForm from './components/LinkForm';
import LinksList from './components/LinksList';
import { getAllLinks } from './services/api';

function App() {
  const [links, setLinks] = useState([]);
  const [showLinksList, setShowLinksList] = useState(false);

  const fetchLinks = async () => {
    try {
      const linksData = await getAllLinks();
      setLinks(linksData);
    } catch (error) {
      console.error('Error fetching links:', error);
    }
  };
  
  // fetch links when rendering
  useEffect(() => {
    fetchLinks();
  }, []);

  const addLink = (link) => {
    if (!links.some(l => l.id === link.id)){
      setLinks([link, ...links])
    };
  };

  const changeShow = () => {
    setShowLinksList(!showLinksList);
  
    if (!showLinksList){
      fetchLinks();
    }
  }

  return (
    <div className="App">
      <header className="App-header">
        <h1>Link Shortener</h1>
        <div className='main-bar'>
          <div className='list-bar'> <button className="linksListButton" onClick={changeShow}>List links</button> </div>
          <LinkForm onAddLink={addLink} onSet/>
          
        </div>
        { showLinksList && <LinksList links={links}/>}
      </header>
    </div>
  );
}

export default App;
