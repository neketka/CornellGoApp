import { createContext, useState, useRef, useEffect, ReactNode } from "react";
import * as signalR from "@microsoft/signalr";

export type ServerPlace = {
  id: string;
  imageUrl: string;
  name: string;
  description: string;
  points: number;
  lat: number;
  long: number;
  radius: number;
  citationUrl: string;
  linkUrl: string;
  longDescription: string;
};

export type ServerDataType = {
  places: ServerPlace[];
  unapprovedAdmins: string[];
  loggedIn: boolean;
  addPlace: (place: ServerPlace) => void;
  deletePlace: (id: string) => void;
  modifyPlace: (id: string, place: ServerPlace) => void;
  updateAdmin: (name: string, approve: boolean) => void;
  login: (name: string, password: string) => Promise<ServerLoginResult>;
  requestAdmin: (name: string, password: string) => Promise<boolean>;
};

export enum ServerLoginResult {
  NoAdminApproval = 0,
  AdminRejected = 1,
  NoAccount = 2,
  WrongPassword = 3,
  Success = 4,
}

export enum PlaceDataModifiedState {
  Created = 0,
  Destroyed = 1,
  Modified = 2,
}

export const ServerDataContext = createContext({
  places: [],
  unapprovedAdmins: [],
  loggedIn: false,
  addPlace: (p) => {},
  deletePlace: (id) => {},
  modifyPlace: (i, p) => {},
  updateAdmin: (name, approve) => {},
  login: async (name, password) => ServerLoginResult.AdminRejected,
  requestAdmin: async (name, password) => false,
} as ServerDataType);

export function ServerData(props: { children: ReactNode }) {
  const [places, setPlaces] = useState([] as ServerPlace[]);
  const [unapprovedAdmins, setUnapprovedAdmins] = useState([] as string[]);
  const [loggedIn, setLoggedIn] = useState(false);

  const connectionRef = useRef<signalR.HubConnection>();

  const methodRef = useRef({
    modified: (state: PlaceDataModifiedState, data: ServerPlace) => {},
    approval: (email: string, approval: boolean) => {},
  });

  const placesRef = useRef([] as ServerPlace[]);
        
  useEffect(() => {
    methodRef.current.modified = (
      state: PlaceDataModifiedState,
      data: ServerPlace
    ) => {
      switch (state) {
        case PlaceDataModifiedState.Created:
          setPlaces([data, ...places]);
          break;
        case PlaceDataModifiedState.Destroyed:
          setPlaces(places.filter((place) => place.id !== data.id));
          break;
        case PlaceDataModifiedState.Modified:
          {
            const idx = places.findIndex((place) => place.id === data.id);
            console.log(idx, places);
            if (idx >= 0) {
              places[idx] = data;
              setPlaces([...places]);
            }
          }
          break;
      }
    };

    methodRef.current.approval = (email: string, approved: boolean) => {
      if (approved)
        setUnapprovedAdmins(unapprovedAdmins.filter((admin) => admin !== email));
      else
        setUnapprovedAdmins([...unapprovedAdmins, email]);
    };
  });

  useEffect(() => {
    connectionRef.current = new signalR.HubConnectionBuilder()
      .withUrl("/adminhub")
      .withAutomaticReconnect()
      .build();

    connectionRef.current.on(
      "PlaceModified",
      (state: PlaceDataModifiedState, data: ServerPlace) =>
        methodRef.current.modified(state, data)
    );
    connectionRef.current.on(
      "AdminApprovalUpdate",
      (email: string, approval: boolean) =>
        methodRef.current.approval(email, approval)
    );
    connectionRef.current.start().catch((err) => console.log(err));
  }, []);

  const data: ServerDataType = {
    places,
    unapprovedAdmins,
    loggedIn,
    addPlace: (p) => {
      connectionRef.current
        ?.invoke("ModifyPlace", PlaceDataModifiedState.Created, p)
        .then(setLoggedIn);
    },
    deletePlace: (id) => {
      connectionRef.current
        ?.invoke("ModifyPlace", PlaceDataModifiedState.Destroyed, {
          id: id,
          imageUrl: "",
          name: "",
          description: "",
          points: 0,
          lat: 0,
          long: 0,
          radius: 0,
          citationUrl: "",
          linkUrl: "",
          longDescription: "",
        } as ServerPlace)
        .then(setLoggedIn);
    },
    modifyPlace: (id, p) => {
      connectionRef.current
        ?.invoke("ModifyPlace", PlaceDataModifiedState.Modified, {
          ...p,
          id: id,
        } as ServerPlace)
        .then(setLoggedIn);
    },
    updateAdmin: (name, approve) => {
      connectionRef.current
        ?.invoke("UpdateAdminStatus", name, approve)
        .then(setLoggedIn);
    },
    login: async (name, password) => {
      const result = await connectionRef.current?.invoke(
        "Login",
        name,
        password
      );
      if (result === ServerLoginResult.Success) {
        placesRef.current = [];
        connectionRef.current?.stream("GetPlaces").subscribe({
          next: (place) => {
            console.log(place);
            placesRef.current.push(place);
            },
          complete: () => setPlaces(placesRef.current.reverse()),
          error: (err) => {},
        });

        connectionRef.current
          ?.invoke("GetUnapprovedAdmins")
          .then((admins: string[]) =>
            setUnapprovedAdmins([...admins, ...unapprovedAdmins])
          );
        setLoggedIn(true);
      }
      return result;
    },
    requestAdmin: async (name, password) => {
      return (await connectionRef.current?.invoke(
        "RequestAdmin",
        name,
        password
      )) as boolean;
    },
  };
  return (
    <ServerDataContext.Provider value={data}>
      {props.children}
    </ServerDataContext.Provider>
  );
}
