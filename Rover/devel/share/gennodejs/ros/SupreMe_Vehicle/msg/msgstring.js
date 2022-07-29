// Auto-generated. Do not edit!

// (in-package SupreMe_Vehicle.msg)


"use strict";

const _serializer = _ros_msg_utils.Serialize;
const _arraySerializer = _serializer.Array;
const _deserializer = _ros_msg_utils.Deserialize;
const _arrayDeserializer = _deserializer.Array;
const _finder = _ros_msg_utils.Find;
const _getByteLength = _ros_msg_utils.getByteLength;

//-----------------------------------------------------------

class msgstring {
  constructor(initObj={}) {
    if (initObj === null) {
      // initObj === null is a special case for deserialization where we don't initialize fields
      this.msgtext = null;
    }
    else {
      if (initObj.hasOwnProperty('msgtext')) {
        this.msgtext = initObj.msgtext
      }
      else {
        this.msgtext = '';
      }
    }
  }

  static serialize(obj, buffer, bufferOffset) {
    // Serializes a message object of type msgstring
    // Serialize message field [msgtext]
    bufferOffset = _serializer.string(obj.msgtext, buffer, bufferOffset);
    return bufferOffset;
  }

  static deserialize(buffer, bufferOffset=[0]) {
    //deserializes a message object of type msgstring
    let len;
    let data = new msgstring(null);
    // Deserialize message field [msgtext]
    data.msgtext = _deserializer.string(buffer, bufferOffset);
    return data;
  }

  static getMessageSize(object) {
    let length = 0;
    length += object.msgtext.length;
    return length + 4;
  }

  static datatype() {
    // Returns string type for a message object
    return 'SupreMe_Vehicle/msgstring';
  }

  static md5sum() {
    //Returns md5sum for a message object
    return 'bf820a8e464d1831c7daa93d2905b133';
  }

  static messageDefinition() {
    // Returns full string definition for message
    return `
    string msgtext
    
    `;
  }

  static Resolve(msg) {
    // deep-construct a valid message object instance of whatever was passed in
    if (typeof msg !== 'object' || msg === null) {
      msg = {};
    }
    const resolved = new msgstring(null);
    if (msg.msgtext !== undefined) {
      resolved.msgtext = msg.msgtext;
    }
    else {
      resolved.msgtext = ''
    }

    return resolved;
    }
};

module.exports = msgstring;
