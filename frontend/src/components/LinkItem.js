import React from 'react';

const LinkItem = ({ link }) => {
  return (
    <tr>
        <td> {link.id} </td>
        <td> <a href={link.originalUrl}>{link.originalUrl}</a> </td>  
        <td> {link.hash} </td> 
        <td> {link.createdAt} </td>
        <td> {link.accessCount} </td>
    </tr>
  );
};

export default LinkItem;
