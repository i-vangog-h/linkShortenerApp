import React, { useState } from 'react';
import { generateShortLink } from '../services/api';
import ShortenedDisplay from './ShortenedDisplay';

const LinkForm = ({ onAddLink }) => {
  const [url, setUrl] = useState('');
  const [shortenedUrl, setShortenedUrl] = useState('');
  const [errrorMessage, setErrorMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (url){ // is not null
      try{
        const response = await generateShortLink( url )
        setErrorMessage('');
        setShortenedUrl(response.shortUrl);

        //callback function, adds new link to the list of links on App.js
        if (shortenedUrl) onAddLink(shortenedUrl);
        
        console.log(shortenedUrl);
      }
      catch (error){
        console.error("Error creating shortened link: ", error);
        setShortenedUrl('');
        if(error.response){
          setErrorMessage(error.response.data);
        }
        else{
          setErrorMessage(error.message);
        }
      }
    }
  };

  const copyToClipboard = async (text) => {
    try {
      await navigator.clipboard.writeText(text);
    } catch (err) {
      console.error('Failed to copy: ', err);
    }
  };

  return (
    <div className='formContainer'>
      <form onSubmit={handleSubmit}>
        <input
          type="url"
          value={url}
          onChange={(e) => setUrl(e.target.value)}
          placeholder="Enter your URL"
          required
        />
        <button type="submit">Shorten</button>
      </form>
      {errrorMessage && (
        <div className='errorBlock'>
          {errrorMessage}
        </div>
      )}
      
      {shortenedUrl && ( //if shortenedUrl is not null then display
        <ShortenedDisplay 
          clearClick={() => {setUrl(''); setShortenedUrl('');} } 
          copyClick={() => copyToClipboard(shortenedUrl)}
          shortenedUrl={shortenedUrl} 
        />
      )}
      
    </div>
  );
};

export default LinkForm;
