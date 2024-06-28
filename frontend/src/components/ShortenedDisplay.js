

const ShortenedDisplay = ({ clearClick, copyClick, shortenedUrl }) => {
    return(
        <div className='shortLinkDisplay'>
          <button className='clear' onClick={clearClick}> clear </button>
          <button className='copy' onClick={copyClick}> copy </button>
          <a href={shortenedUrl}> {shortenedUrl} </a>
        </div>
    )
}

export default ShortenedDisplay;