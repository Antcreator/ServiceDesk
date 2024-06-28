import React from "react";
import { Outlet, useNavigation } from "react-router-dom";

export default function Root() {
  const navigation = useNavigation();
  
  return (
    <>
      <div 
        style={{
          visibility: navigation.state === 'loading' ? 'visible' : 'hidden'
        }}>
        Loading...
      </div>
      <Outlet />
    </>
  );
}
