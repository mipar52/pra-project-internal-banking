import React from "react";
import { Modal, Button } from "react-bootstrap";

interface DeleteFriendModalProps {
  show: boolean;
  onClose: () => void;
  onConfirm: () => void;
  friendName: string;
}

const DeleteFriendModal: React.FC<DeleteFriendModalProps> = ({
  show,
  onClose,
  onConfirm,
  friendName,
}) => {
  return (
    <Modal show={show} onHide={onClose} centered backdrop="static">
      <Modal.Header closeButton className="bg-danger text-white">
        <Modal.Title>Confirm Delete</Modal.Title>
      </Modal.Header>
      <Modal.Body className="text-center">
        <p>
          Are you sure you want to remove <strong>{friendName}</strong> from
          your friend list?
        </p>
      </Modal.Body>
      <Modal.Footer className="d-flex justify-content-between">
        <Button variant="secondary" onClick={onClose}>
          Cancel
        </Button>
        <Button variant="danger" onClick={onConfirm}>
          Yes, Delete
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default DeleteFriendModal;
