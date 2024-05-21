import { Room, Client, Delayed } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";
import axios from "axios";
import { Library } from "../Library";

export class Player extends Schema {
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;
    playersDeck = new Map();

    onCreate(options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            //this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("spawn", (client, data) => {
            const spawnData = JSON.parse(data.json);

            if (this.playersDeck.get(client.id).includes(spawnData.cardID)) {
                spawnData.serverTime = this.clock.elapsedTime + 1000;
                const json = JSON.stringify(spawnData);
                client.send("SpawnPlayer", json);
                this.broadcast("SpawnEnemy", json, { except: client });
            }
            else {
                client.send("Cheat");
            }
        });
    }

    gameIsStarted: boolean = false;
    awaitStart: Delayed;
    startTimer:Delayed;
    tickCount = 0;

    async onJoin(client: Client, data) {
        try {
            const response = await axios.post(Library.getDeckURI, { key: Library.phpKEY, userID: data.id });
            console.log(response.data);
            this.playersDeck.set(client.id, response.data);
        } catch (error) {
            console.log("Вылетела ошибка " + error);
        }

        this.state.createPlayer(client.sessionId);

        if (this.clients.length < 2) return;

        this.broadcast("GetReady");

        this.awaitStart = this.clock.setTimeout(() => {
            try {
                //this.broadcast("Start", JSON.stringify({ player1ID: this.clients[0].id, player1: this.playersDeck.get(this.clients[0].id), player2: this.playersDeck.get(this.clients[0].id) }));
                this.broadcast("Start", JSON.stringify({ player1ID: this.clients[0].id, player1: this.playersDeck.get(this.clients[0].id), player2: this.playersDeck.get(this.clients[1].id) }));

                this.gameIsStarted = true;
            } catch (error) {
                this.broadcast("CancelStart");
            }
        }, 1000);

        this.startTimer = this.clock.setInterval(() => {
            this.tickCount++;
            this.broadcast("StartTick", JSON.stringify({ tick: this.tickCount, time: this.clock.elapsedTime }));
            if (this.tickCount > 9) this.startTimer.clear();
        }, 1000)
    }

    onLeave(client) {
        if (this.gameIsStarted === false && this.awaitStart !== undefined && this.awaitStart.active) {
            this.broadcast("CancelStart");
            this.awaitStart.clear();
        }

        if (this.playersDeck.has(client.id))
            this.playersDeck.delete(client.id);

        this.state.removePlayer(client.sessionId);
    }

    onDispose() {

    }

}