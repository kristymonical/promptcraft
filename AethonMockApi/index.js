"use strict";

const Hapi = require("@hapi/hapi");
const Boom = require("@hapi/boom");

const init = async () => {
  const server = Hapi.server({
    port: 3000,
    host: "localhost"
  });

  async function wait(ms) {
    return new Promise(resolve => {
      setTimeout(resolve, ms);
    });
  }

  const getNewId = (() => {
    const _newId = () => Math.floor(Math.random() * (Date.now() / 1000));
    const _ids = [];
    return () => {
      let _id = _newId();
      while (_ids.includes(_id)) {
        _id = _newId();
      }
      return _id;
    };
  })();

  const jobs = {};
  const destinationLookup = {
    "101B-FPA-001": 1,
    "101B-FPA-002": 2,
    "101B-FPA-003": 3,
    "101B-FPA-004": 4,
    "101B-FPA-005": 5,
    "101C-CW-001": 6,
    "101C-CW-002": 7,
    "101C-CW-003": 8,
    "101C-CW-004": 9,
    "101C-CW-005": 10,
    "1430-STG-001": 11,
    "1430-STG-002": 12,
    "1430-STG-003": 13,
    "1430-STG-004": 14,
    "1430-STG-005": 15,
    "1430-STG-006": 16,
    "1430-STG-007": 17,
    "1430-STG-008": 18,
    "1430-STG-009": 19,
    "1430-STG-010": 20,
    "1501-MAL-A-001": 21,
    "1501-MAL-A-WAIT-001": 22,
    "1501-MAL-A-WAIT-002": 23,
    "1501-MAL-A-WAIT-003": 24,
    "2501-MAL-A-001": 25,
    "2501-MAL-A-WAIT-001": 26,
    "2501-MAL-A-WAIT-002": 27,
    "2501-MAL-A-WAIT-003": 28,
    "1501-MAL-B-001": 29,
    "1501-MAL-B-WAIT-001": 30,
    "1501-MAL-B-WAIT-002": 31,
    "1501-MAL-B-WAIT-003": 32,
    "2501-MAL-B-001": 33,
    "2501-MAL-B-WAIT-001": 34,
    "2501-MAL-B-WAIT-002": 35,
    "2501-MAL-B-WAIT-003": 36,
    "1528-MAL-001": 37,
    "1528-MAL-WAIT-001": 38,
    "1528-MAL-WAIT-002": 39,
    "1528-MAL-WAIT-003": 40,
    "2519-MAL-001": 41,
    "2519-MAL-WAIT-001": 42,
    "2519-MAL-WAIT-002": 43,
    "2519-MAL-WAIT-003": 44
  };

  async function send({
    payload: { gid = 1, pid, destinations },
    query: { errorTest = false }
  }) {
    if (errorTest) throw Boom.badRequest();

    await wait(1000);
    const jid = getNewId();
    jobs[jid] = {
      jid,
      pid,
      gid,
      tid: 1,
      jobType: "API",
      state: "SCHEDULED",
      itinerary: destinations.map(val => ({
        rid: getNewId(),
        did: destinationLookup[val],
        destination: val,
        state: "SCHEDULED"
      }))
    };

    return { code: true, jid };
  }

  server.route({
    method: "PUT",
    path: "/job/{jid}/complete/",
    handler: async ({ params: { jid } }) => {
      const job = jobs[jid];
      if (!job) throw Boom.notFound("JobNotFound");

      job.state = "COMPLETED";
      job.end = new Date().toISOString();

      await job.itinerary.forEach(async itnry => {
        await server.inject({
          method: "PUT",
          url: `/job/${jid}/itinerary/${itnry.rid}/complete/`
        });
      });

      return { success: true, message: "" };
    }
  });

  server.route({
    method: "PUT",
    path: "/job/complete/",
    handler: async () => {
      await Object.keys(jobs).forEach(
        async job =>
          await server.inject({
            method: "PUT",
            url: `/job/${job}/complete/`
          })
      );

      return { success: true, message: "" };
    }
  });

  server.route({
    method: "PUT",
    path: "/job/cancel/",
    handler: async () => {
      await Object.keys(jobs).forEach(
        async job =>
          await server.inject({
            method: "PUT",
            url: `/job/${job}/cancel/`
          })
      );

      return { success: true, message: "" };
    }
  });

  server.route({
    method: "PUT",
    path: "/job/{jid}/cancel/",
    handler: ({ params: { jid } }) => {
      const job = jobs[jid];
      if (!job) throw Boom.notFound("JobNotFound");

      job.state = "CANCELED";
      job.end = new Date().toISOString();

      return { success: true, message: "" };
    }
  });

  server.route({
    method: "PUT",
    path: "/job/{jid}/expire/",
    handler: ({ params: { jid } }) => {
      if (jobs[jid]) jobs[jid].state = "EXPIRED";
      return { success: true, message: "" };
    }
  });

  server.route({
    method: "PUT",
    path: "/job/{jid}/itinerary/{rid}/complete/",
    handler: ({ params: { jid, rid } }) => {
      if (jobs[jid] && jobs[jid].itinerary && jobs[jid].itinerary.length) {
        const idx = jobs[jid].itinerary.findIndex(el => el.rid == rid);

        if (typeof idx == "number" && idx >= 0) {
          jobs[jid].itinerary[idx].state = "COMPLETED";
          jobs[jid].itinerary[idx].end = new Date().toISOString();
        }
      }
      return { success: true, message: "" };
    }
  });

  server.route({
    method: "PUT",
    path: "/job/{jid}/itinerary/{rid}/timeout/",
    handler: ({ params: { jid, rid } }) => {
      if (jobs[jid] && jobs[jid].itinerary && jobs[jid].itinerary.length) {
        const idx = jobs[jid].itinerary.findIndex(el => el.rid === rid);
        jobs[jid].itinerary[idx].state = "TIMED_OUT";
      }
      return { success: true, message: "" };
    }
  });

  server.route({
    method: "GET",
    path: "/jobs/",
    handler: () => Object.values(jobs || [])
  });

  server.route({
    method: "POST",
    path: "/activatearea",
    handler: request => {
      const operator = request.payload.amount >= 0 ? "increment" : "decrement";
      return {
        code: true,
        msg: `Successfully ${operator}ed area ${
          request.payload.aid
        } active amount by ${Math.abs(request.payload.amount)}`
      };
    }
  });

  const availablePool = {
    available: 3,
    ids: ["TUG-0-1", "TUG-0-2", "TUG-0-3"]
  };

  const availableRobots = [
    { tid: "TUG-0-1", available: true },
    { tid: "TUG-0-2", available: false },
    { tid: "TUG-0-3", available: true }
  ];

  server.route({
    method: "GET",
    path: "/availability/{id}/{gid?}",
    handler: ({ params: { id } }) =>
      /^\d+$/.test(id) ? availablePool : [{ available: true, tid: id }]
  });

  server.route({
    method: "GET",
    path: "/availability/",
    handler: () => availableRobots
  });

  server.route({
    method: "POST",
    path: "/canceljob/{jid}",
    handler: request => {
      return {
        code: true,
        msg: `Successfully canceled job ${request.params.jid}`
      };
    }
  });

  server.route({
    method: "POST",
    path: "/clear/{tid}",
    handler: request => {
      return {
        code: true,
        tid: request.params.tid
      };
    }
  });

  server.route({
    method: "GET",
    path: "/destination",
    handler: () => [
      {
        did: 1,
        name: "2South",
        alias: "2 South",
        type: "AREA",
        aid: 7,
        gid: 4,
        enabled: 1
      }
    ]
  });

  server.route({
    method: "POST",
    path: "/drive",
    handler: ({ payload: { tid } }) => ({
      code: true,
      message: `Succesfully forwarded drive command to ${tid}`,
      tid: `${tid}`
    })
  });

  server.route({
    method: "POST",
    path: "/estop/{tid}",
    handler: ({ params: { tid } }) => ({
      code: true,
      tid: `${tid}`
    })
  });

  server.route({
    method: "GET",
    path: "/area",
    handler: () => [
      {
        aid: 1,
        gid: 4,
        name: "2South",
        active: true,
        charger: true,
        sender: true
      },
      {
        aid: 2,
        gid: 4,
        name: "1East",
        active: true,
        charger: false,
        sender: true
      },
      {
        aid: 3,
        gid: 4,
        name: "2North",
        active: false,
        charger: true,
        sender: false
      }
    ]
  });

  server.route({
    method: "GET",
    path: "/job/{id?}",
    handler: ({ params: { id } }) => {
      if (id) {
        const job = jobs[id];
        if (!job) throw Boom.notFound("JobNotFound");
        return [job];
      }
      return (
        Object.values(jobs).filter(
          job =>
            job.state != "COMPLETED" &&
            job.state != "CANCELED" &&
            job.state != "EXPIRED"
        ) || []
      );
    }
  });

  server.route({
    method: "GET",
    path: "/pool",
    handler: () => [
      {
        pid: 1,
        name: "Pool 1",
        ctid: 4
      }
    ]
  });

  server.route({
    method: "GET",
    path: "/robot/{tid?}",
    handler: ({ params: { tid = "TUG-0-1" } }) => [
      {
        tid,
        name: `Robot: ${tid}`,
        ctid: 1,
        pid: 5,
        enabled: 1
      }
    ]
  });

  server.route({
    method: "GET",
    path: "/photo/{tid}/{width}/{height}/{base64}",
    handler: ({ params: { tid = "TUG-0-1" } }) => ({
      code: 0,
      tid,
      width: 320,
      height: 240,
      base64: 1,
      data: "image/ png; base64, iVBORw0KGgoAAAANS..."
    })
  });

  server.route({
    method: "GET",
    path: "/status/{tid?}",
    handler: async ({ params: { tid = "TUG-0-1" } }) => {
      // await wait(1000);
      return [
        {
          tid,
          state: "CHARGING",
          status: "Charged on Tug-0-1",
          xCoordinate: 363.305,
          yCoordinate: -136.864,
          heading: 1.6033,
          battery: 100,
          jid: 12385,
          pid: 3,
          executionState: 0,
          errorState: 0,
          obstacleState: "CLEAR",
          liftPosition: "DOWN",
          cartDetected: "OFF",
          idleTime: 0,
          lastCommunicationTime: 0,
          atDestination: 0,
          available: 1
        }
      ];
    }
  });

  server.route({
    method: "GET",
    path: "/types",
    handler: () => [
      {
        ctid: 1,
        name: "cec",
        width: 33,
        length: 66,
        frontToPost: 0,
        postToCenterLine: 36,
        wheelBase: 33
      }
    ]
  });

  server.route({
    method: "GET",
    path: "/unit",
    handler: () => [
      {
        unit: "2S",
        room: "201",
        did: 2,
        hotpoint: "2South"
      }
    ]
  });

  server.route({
    method: "GET",
    path: "/go/{tid}",
    handler: ({ params: { tid = "TUG-0-1" } }) => ({
      code: 1,
      tid
    })
  });

  server.route({
    method: "POST",
    path: "/send/{tid}/{did}",
    handler: () => ({
      code: 1,
      jid: 987453
    })
  });

  server.route({
    method: "POST",
    path: "/robot",
    handler: ({ payload: { name = "Default Name", tid = "TUG-0-1" } }) => ({
      code: 1,
      tid,
      name,
      pid: 5,
      enabled: 1
    })
  });

  server.route({
    method: "POST",
    path: "/pause/{tid}",
    handler: ({ params: { tid = "TUG-0-1" } }) => ({
      code: 1,
      tid
    })
  });

  server.route({
    method: "POST",
    path: "/reposition",
    handler: ({ params: { tid } }) => ({
      code: 1,
      tid
    })
  });

  server.route({
    method: "POST",
    path: "/continue/{tid}",
    handler: ({ params: { tid = "TUG-0-1" } }) => ({
      code: 1,
      tid
    })
  });

  server.route({
    method: "POST",
    path: "/button/{tid}/{type}",
    handler: ({ params: { tid = "TUG-0-1", type = 0 } }) => ({
      code: 1,
      tid,
      type
    })
  });

  server.route({
    method: "POST",
    path: "/sendhome/{tid}",
    handler: ({ params: { tid = "TUG-0-1", type = 0 } }) => ({
      code: 1,
      msg: `Successfully routed Tug: ${tid}`
    })
  });

  server.route({
    method: "POST",
    path: "/send",
    handler: send
  });

  server.events.on("response", request =>
    console.log(
      "\n\nIncoming Request:",
      JSON.stringify(
        {
          remoteAddress: request.info.remoteAddress,
          timestamp: request.info.received,
          route: `${request.method.toUpperCase()} ${request.path}`,
          statusCode: request.response.statusCode,
          requestPayload: request.payload
        },
        null,
        2
      ),
      "\n\n"
    )
  );

  await server.start();
  console.log(`Server running on ${server.info.uri}`);
};

process.on("unhandledRejection", e => {
  console.log(e);
  process.exit();
});

init();
